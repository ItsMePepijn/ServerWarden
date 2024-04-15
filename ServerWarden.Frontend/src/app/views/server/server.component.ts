import { Component, OnDestroy, OnInit } from '@angular/core';
import { ServerService } from '../../services/server.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, Subscription, catchError, filter, map, of, take } from 'rxjs';
import { ServerProfile, ServerUser } from '../../models/server';
import { SignalrHubService } from '../../services/signalr-hub.service';
import { User } from '../../models/auth';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-server',
  templateUrl: './server.component.html',
  styleUrl: './server.component.scss'
})
export class ServerComponent implements OnInit, OnDestroy{
  private paramMapSubscription: Subscription | undefined;
  private connectionSubscription: Subscription | undefined;

  public user$: Observable<User | null> = this.authService.state$.pipe(
    filter(state => !!state && state.user !== null),
    map(state => state ? state.user : null)
  );
  
  public server$: BehaviorSubject<ServerProfile | null> = new BehaviorSubject<ServerProfile | null>(null);

  public serverLogLines: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private serverService: ServerService,
    private authService: AuthService,
    private signalRService: SignalrHubService,
  ) {}

  ngOnInit() {
    this.paramMapSubscription = this.route.paramMap
      .subscribe(paramMap => {
        const id = paramMap.get('id') || "";
        this.fetchServerData(id);

        this.connectionSubscription = this.signalRService.startConnection().pipe(
          catchError(error => {
            console.error('Connection failed!', error);
            return of(undefined);
          })
        ).subscribe(() => {
          // Join server group
          this.signalRService.joinServerGroup(id).then(response => {
            if(!response.success){
              console.error('Failed to join SignalR server group:', response.message);
              this.router.navigate(['/']);
              return;
            }
            console.log('Joined SignalR server group ' + id);
          });

          // Listen for server logs
          this.signalRService.onServerInstallLog$.subscribe(log => {
            console.log(log);
            this.serverLogLines.push(log);
          });

          // Listen for server installation start
          this.signalRService.onServerStartedInstalling$.subscribe(() => {
            this.fetchServerData();

            console.log('Server started installing');
            this.serverLogLines = [];
            this.serverLogLines.push('Server started installing');
          });

          // Listen for server installation finish
          this.signalRService.onServerFinishedInstalling$.subscribe(() => {
            this.fetchServerData();
          });
        });
      });
  }

  public startInstallation(): void {
    this.serverService.startInstallation(this.server$.value?.id || "")
      .subscribe();
  }

  public fetchServerData(id = this.server$.value?.id || ""): void {
    this.serverService.getServerById(id)
      .subscribe(response => {
        if(!response.success){
          this.router.navigate(['/']);
          return;
        }

        this.user$.pipe(
          take(1)
        ).subscribe((user) => {
          response.data.users.filter((u: ServerUser) => u.user.id === user?.id).forEach((u: ServerUser) => u.user.name += " (You)");
        });

        this.server$.next(response.data);
      });
  }

  ngOnDestroy() {
    this.signalRService.disconnect();
    this.connectionSubscription?.unsubscribe();
    this.paramMapSubscription?.unsubscribe();
  }
}

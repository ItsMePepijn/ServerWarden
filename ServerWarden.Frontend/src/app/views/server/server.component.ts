import { Component, OnDestroy, OnInit } from '@angular/core';
import { ServerService } from '../../services/server.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';
import { ServerProfile } from '../../models/server';

@Component({
  selector: 'app-server',
  templateUrl: './server.component.html',
  styleUrl: './server.component.scss'
})
export class ServerComponent implements OnInit, OnDestroy{
  private paramMapSubscription: Subscription | undefined;

  public server$: BehaviorSubject<ServerProfile | null> = new BehaviorSubject<ServerProfile | null>(null);
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private serverService: ServerService
  ) {}

  ngOnInit() {
    this.paramMapSubscription = this.route.paramMap
      .subscribe(paramMap => {
        const id = paramMap.get('id') || "";

        this.serverService.getServerById(id)
          .subscribe(response => {
            if(!response.success){
              this.router.navigate(['/']);
              return;
            }
            this.server$.next(response.data);
          });
      });
  }

  ngOnDestroy() {
    if (this.paramMapSubscription)
      this.paramMapSubscription.unsubscribe();
  }
}

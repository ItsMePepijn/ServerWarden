import { Component } from '@angular/core';
import { ServerService } from '../../services/server.service';
import { Observable, map } from 'rxjs';
import { ServerProfile } from '../../models/server';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  public serverList$: Observable<ServerProfile[]> = new Observable<ServerProfile[]>();
  constructor(
    public serverService: ServerService
  ) {}

  ngOnInit() {
    this.serverList$ = this.serverService.servers$.pipe(
        map(servers => servers || [])
      );

    this.serverService.fetchServerList().subscribe();
  }
}

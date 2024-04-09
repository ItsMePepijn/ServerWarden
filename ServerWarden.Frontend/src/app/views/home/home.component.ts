import { Component } from '@angular/core';
import { ServerService } from '../../services/server.service';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { ServerProfile } from '../../models/server';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  public serverList$: BehaviorSubject<ServerProfile[]> = new BehaviorSubject<ServerProfile[]>([]);
  constructor(
    public serverService: ServerService
  ) {}

  ngOnInit() {
    this.serverService.getServerList()
      .pipe(
        map(response => response.data || [])
      )
      .subscribe(data => {
        this.serverList$.next(data);
      });
  }
}

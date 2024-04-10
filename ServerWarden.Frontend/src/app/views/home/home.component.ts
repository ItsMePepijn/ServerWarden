import { Component } from '@angular/core';
import { ServerService } from '../../services/server.service';
import { BehaviorSubject, map } from 'rxjs';
import { ServerProfileSimple } from '../../models/server';
import { ModalService } from '../../services/modal.service';
import { ModalType } from '../../models/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  public serverList$: BehaviorSubject<ServerProfileSimple[]> = new BehaviorSubject<ServerProfileSimple[]>([]);

  constructor(
    private serverService: ServerService,
    private modalService: ModalService
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

  public newServer() {
    this.modalService.openModal(ModalType.NewServer);
  }
}

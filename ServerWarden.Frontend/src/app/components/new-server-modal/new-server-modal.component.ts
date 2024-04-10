import { Component, OnDestroy, OnInit } from '@angular/core';
import { ModalService } from '../../services/modal.service';
import { Subscription } from 'rxjs';
import { ModalType } from '../../models/common';
import { ServerType } from '../../models/server';
import { ServerService } from '../../services/server.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-server-modal',
  templateUrl: './new-server-modal.component.html',
  styleUrl: './new-server-modal.component.scss'
})
export class NewServerModalComponent implements OnInit, OnDestroy{
  private modalSubscription: Subscription | undefined;
  public opened: boolean = false;
  public error: boolean = false;
  public serverTypes = Object.keys(ServerType)
    .filter(key => isNaN(Number(key)))
    .map((key, index) => {
      return { id: index, name: key };
    });

  public selectedServerType: ServerType | null = null;
  public serverName: string = '';

  constructor(
    private modalService: ModalService,
    private serverService: ServerService,
    private router: Router
  ) {}

  ngOnInit() {
    this.modalSubscription = this.modalService.modalOpened$.subscribe(modalType => {
      if(modalType === ModalType.NewServer){
        this.opened = true;

        this.selectedServerType = null;
        this.serverName = '';
        this.error = false;
      }
    });
  }

  public closeModal() {
    this.opened = false;
  }

  public createServer() {
    this.error = false;
    if(this.selectedServerType == null || !this.serverName){
      this.error = true;
      return;
    }

    this.serverService.createServer(this.selectedServerType, this.serverName)
      .subscribe(response => {
        if(response.success){
          this.closeModal();
          this.router.navigate(['/server', response.data.id]);
        } else {
          this.error = true;
        }
      });
  }

  ngOnDestroy() {
    if (this.modalSubscription)
      this.modalSubscription.unsubscribe();
  }
}

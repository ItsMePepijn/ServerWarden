import { EventEmitter, Injectable } from '@angular/core';
import { ModalType } from '../models/common';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  public modalOpened$: EventEmitter<ModalType> = new EventEmitter<ModalType>();

  public openModal(modalType: ModalType) {
    this.modalOpened$.emit(modalType);
  }
}
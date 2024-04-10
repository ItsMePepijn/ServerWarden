import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DropdownOption } from '../../models/common';

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss'
})
export class DropdownComponent {
  @Input() public options: DropdownOption[] = [];
  @Input() public imagePath: string | null = null;
  @Output() public selected = new EventEmitter<number>();

  public selectedOption: DropdownOption | null = null;
  public opened: boolean = false;

  public selectOption(item: number) {
    this.selected.emit(item);
    this.selectedOption = this.options[item];
    this.opened = false;
  }

  public toggleDropdown() {
    this.opened = !this.opened;
  }
}

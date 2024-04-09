import { Component } from '@angular/core';
import { ChildrenOutletContexts } from '@angular/router';
import { authSlideAnimation } from '../../animations';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss',
  animations: [
    authSlideAnimation
  ]
})
export class AuthComponent {
  constructor(
    private contexts: ChildrenOutletContexts
  ) {}

  getRouteAnimationData() {
    return this.contexts.getContext('primary')?.route?.snapshot?.data?.['animation'];
  }
}

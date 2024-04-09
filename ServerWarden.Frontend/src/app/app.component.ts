import { Component } from '@angular/core';
import { ChildrenOutletContexts } from '@angular/router';
import { slideAnimation } from './animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  animations: [
    slideAnimation
  ]
})
export class AppComponent {
  title = 'Server Warden';

  constructor(
    private contexts: ChildrenOutletContexts
  ) {}

  getRouteAnimationData() {
    return this.contexts.getContext('primary')?.route?.snapshot?.data?.['animation'];
  }
}

import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss'
})
export class NavBarComponent implements OnInit{
  public loggedIn: boolean = false;

  constructor(
    public authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.state$.subscribe(state => {
      this.loggedIn = !!state && !!state.token
    });
  }
}

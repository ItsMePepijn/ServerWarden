import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { filter } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  public userName: string | null = null;
  public password: string | null = null;

  public error: string | null = null;

  constructor(
    private authService: AuthService
  ) {}

  public login() {
    if(!this.userName || !this.password){
      this.error = "Please fill out all fields";
      return;
    }
    this.error = null;

    this.authService.login(this.userName, this.password)
      .pipe(
        filter(response => !response.success)
      )
      .subscribe(response => {
        this.error = response.message;
      });
  }
}

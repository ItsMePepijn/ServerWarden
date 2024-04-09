import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { filter } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  public userName: string | null = null;
  public password: string | null = null;
  public passwordRepeat: string | null = null;

  public error: string | null = null;

  constructor(
    private authService: AuthService
  ) {}

  public register() {
    if(!this.userName || !this.password || !this.passwordRepeat){
      this.error = "Please fill out all fields";
      return;
    }
    if(this.password !== this.passwordRepeat){
      this.error = "Make sure the passwords match";
      return;
    }
    this.error = null;

    this.authService.register(this.userName, this.password)
      .pipe(
        filter(response => !response.success)
      )
      .subscribe(response => {
        this.error = response.message;
      });
  }
}

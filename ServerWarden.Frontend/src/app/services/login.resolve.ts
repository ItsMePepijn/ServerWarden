import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, map } from "rxjs";
import { AuthService } from "./auth.service";

@Injectable({ providedIn: 'root' })
export class LoginResolve {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  resolve(): Observable<boolean> {
    return this.authService.state$.pipe(
       map(user => {
          if (user) {
            this.router.navigateByUrl('/');
          }
          return true;
       })
    );
 }
}
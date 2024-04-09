import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, map } from "rxjs";
import { AuthService } from "./auth.service";

@Injectable({ providedIn: 'root' })
export class AuthGuard {

    constructor(
      private authService: AuthService,
      private _router: Router
    ) {}

    canActivate(): Observable<boolean> {
      return this.authService.state$.pipe(
        map(state => {
          // if user does not exists, redirect to login
          if (!state || !state.token) {
            this._router.navigateByUrl('/login');
            return false;
          }
          // user exists
          return true;
       })
      );
    }
}
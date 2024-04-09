import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { ApiResponse } from '../models/common';
import { AuthState } from '../models/auth';
import { Router } from '@angular/router';
// import { SignalrHubService } from './signalr-hub.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private state: BehaviorSubject<AuthState | null> = new BehaviorSubject<AuthState | null>(null);
  public readonly state$: Observable<AuthState | null> = this.state.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
    // private signalRService: SignalrHubService
  ) { }

  login(name: string, password: string): Observable<ApiResponse> {
    return this.http.post<ApiResponse>('/api/auth/login', { name, password }).pipe(
      map((response) => {
        if(response.success){
          this.setState(response.data);
          this.router.navigateByUrl('/');
        }
        return response;
      }),
      catchError((e) => {
        return of(e.error || e);
      })
    );
  }

  logout() {
    this.clearState();
    // this.signalRService.disconnect();
    this.router.navigateByUrl('/login');
  }

  register(name: string, password: string): Observable<ApiResponse> {
    return this.http.post<ApiResponse>('/api/auth/register', { name, password }).pipe(
      map((response) => {
        if(response.success){
          this.setState(response.data);
          this.router.navigateByUrl('/');
        }
        return response;
      }),
      catchError((e) => {
        return of(e.error || e);
      })
    );
  }

  setState(token: string) {
    localStorage.setItem('token', token);
    let state: AuthState = {
      token: token,
      user: this.jwtHelper.decodeToken(token)
    }
    this.state.next(state);
  }
  clearState() {
    localStorage.removeItem('token');
    this.state.next(null);
  }
}

const checkAuth = (token: string | null): boolean => {
  if(!token) return false;
  let jwtHelper = new JwtHelperService();
  
  // Check if the token data is valid for User
  if (!jwtHelper.isTokenExpired(token)) {
    let tokenData = jwtHelper.decodeToken(token);
    if (tokenData && tokenData.name && tokenData.id) {
      return true;
    }
    return false;
  }
  return false;
};

export const authFactory = (authService: AuthService) => () => {
  const token: string | null = localStorage.getItem('token') || null;
  if (checkAuth(token)) {
    authService.setState(token!);
  } else {
    authService.clearState();
  }
};
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { ApiResponse } from '../models/common';
import { ServerType } from '../models/server';

@Injectable({
  providedIn: 'root'
})
export class ServerService {
  constructor(
    private http: HttpClient,
  ) { }

  public getServerList(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>('/api/servers').pipe(
      catchError((e) => {
        console.error(e.error || e);
        return of(e.error || e);
      })
    );
  }

  public getServerById(id: string): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`/api/servers/${id}`).pipe(
      catchError((e) => {
        console.error(e.error || e);
        return of(e.error || e);
      })
    );
  }

  public createServer(serverType: ServerType, name: string): Observable<ApiResponse> {
    return this.http.post<ApiResponse>('/api/servers/',
      {
        serverType,
        name
      }
    ).pipe(
      catchError((e) => {
        console.error(e.error || e);
        return of(e.error || e);
      })
    );
  }
}
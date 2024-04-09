import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { ApiResponse } from '../models/common';
import { ServerProfile } from '../models/server';

@Injectable({
  providedIn: 'root'
})
export class ServerService {
  public readonly servers$: BehaviorSubject<ServerProfile[] | null> = new BehaviorSubject<ServerProfile[] | null>(null);

  constructor(
    private http: HttpClient,
  ) { }

  fetchServerList(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>('/api/servers').pipe(
      map((response) => {
        if(response.success){
          this.servers$.next(response.data);
        }
        return response;
      }),
      catchError((e) => {
        console.error(e.error || e);
        return of(e.error || e);
      })
    );
  }
}
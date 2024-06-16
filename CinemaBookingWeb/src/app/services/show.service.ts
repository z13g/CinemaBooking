import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Show } from '../models/show/show';
import { environment } from 'src/environments/environment.development';

export interface ShowDetailsDTO {
  showId: number;
  cinemaID: number;
  cinemaName: string;
  hallName: string;
  showDateTime: Date;
  movieTitle: string;
}

export interface ShowDateMap {
  [date: string]: ShowDetailsDTO[];
}

export interface CinemaShowMap {
  [cinemaName: string]: ShowDateMap;
}

@Injectable({
  providedIn: 'root'
})
export class ShowService {
  
  private handleError(error: HttpErrorResponse) {
    if (error.status === 404) {
      return throwError(() => new Error('Resource not found'));
    } else {
      return throwError(() => new Error('An unknown error occurred'));
    }
  }

  constructor(private http: HttpClient) { }

  // Get filtered shows based on area and movie
  getFilteredShows(endpoint: string, areaId: number, movieId: number): Observable<CinemaShowMap> {
    console.log("TestService");
    
    return this.http.get<CinemaShowMap>(`${environment.apiUrl}${endpoint}/filtered?areaId=${areaId}&movieId=${movieId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

}

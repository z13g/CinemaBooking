import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Cinema } from '../models/cinema/cinema';

@Injectable({
  providedIn: 'root'
})
export class CinemaService {

  url:string = "https://localhost:7092/api/Cinema"
  constructor(private http: HttpClient) { }
  
  getAll(): Observable<Cinema[]> {
    return this.http.get<Cinema[]>(this.url);
  }
}

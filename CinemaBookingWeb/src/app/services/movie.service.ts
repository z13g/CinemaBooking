import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';
import { Movie } from '../models/movie/movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  url: string = "https://localhost:7092/api/Movie"
  constructor(private http: HttpClient) { }

  getAll(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.url);
  }

  saveMovie(movieData: Movie): Observable<any> {
    return this.http.post(this.url, movieData, { observe: 'response' });
  }
}

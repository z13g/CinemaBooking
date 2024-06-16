import { Injectable } from '@angular/core';
import { Costumer } from '../models/costumer/costumer';
import { HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CostumerService {

  url:string = "https://localhost:7092/api/Costumer"
  constructor(private http: HttpClient) { }

  getAll(): Observable<Costumer[]> {
    return this.http.get<Costumer[]>(this.url);
  }
}

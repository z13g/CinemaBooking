import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Area } from '../models/area/area';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AreaService {

  // url: string = environment.apiUrl + 'Area'
  // constructor(private http: HttpClient) { }

  // getAll(): Observable<Area[]>{
  //   return this.http.get<Area[]>(this.url)
  // }
}

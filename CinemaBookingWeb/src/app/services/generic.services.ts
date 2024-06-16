import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, map } from 'rxjs';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { AuthService } from './auth.service';

interface Area {
  areaID: number;
  areaName: string;
}

const httpOptions = {
  headers: new HttpHeaders({
    'content-type': 'application/json'
  })
};

interface UserDetails {
  username: string;
  role: string;
  authToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class GenericService<Tentity> {

  private isLoggedIn = false; // Sæt denne til true ved login
  private userRole!: string;
  private username!: string;
  private authToken!: string;

  url : string = environment.apiUrl
  constructor(private http : HttpClient, private router: Router) { 

  }

  getAll(endpoint: string): Observable<Tentity[]> {
    return this.http.get<Tentity[]>(`${environment.apiUrl}${endpoint}`, httpOptions).pipe(
      catchError(this.handleError)
      );
  }

  get(endpoint: string): Observable<Tentity> {
    return this.http.get<Tentity>(`${environment.apiUrl}${endpoint}`, httpOptions).pipe(
      catchError(this.handleError)
    );
  }

  getById(endpoint: string, id: number): Observable<Tentity> {
    return this.http.get<Tentity>(`${environment.apiUrl}${endpoint}/${id}`, httpOptions).pipe(
      catchError(this.handleError)
    );
  }

  create(endpoint: string, data: Tentity): Observable<Tentity> {
    console.log("Data in Service: ",data);
    console.log("endoint: ", `${environment.apiUrl}${endpoint}`);
    
    return this.http.post<Tentity>(`${environment.apiUrl}${endpoint}`, data, httpOptions);
  }

  createBulk(endpoint: string, data: Tentity[]): Observable<any> {
    console.log("Data in service: ", data);
    console.log("endpoint: ", endpoint);
    return this.http.post<Tentity[]>(`${environment.apiUrl}${endpoint}/bulk`, data, httpOptions)
  }

  update(endpoint: string, data: Tentity, id:number): Observable<Tentity> {
    if (id === undefined) {
      throw new Error("Cannot update entity without an ID.");
    }
    console.log("id", id);
    console.log("Data: ", data);
    
    return this.http.put<Tentity>(`${environment.apiUrl}${endpoint}/${id}`, data, httpOptions)
  }

  delete(endpoint: string, id: number): Observable<boolean> {
    return this.http.delete(`${environment.apiUrl}${endpoint}/${id}`, httpOptions).pipe(
      map(() => true)
    );
  }

  exists(endpoint: string, id: number): Observable<Area | boolean> { 
    return this.http.get<Area | boolean>(`${environment.apiUrl}${endpoint}/${id}`);
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 404) {
      return throwError(() => new Error('Resource not found'));
    } else {
      return throwError(() => new Error('An unknown error occurred'));
    }
  }

}

@Injectable({
  providedIn: 'root'
})
export class LocalStorageGeneric {
  constructor(private service: GenericService<any>, private router: Router) {
  }
  async handleLocalStorage(): Promise<boolean> {
    const selectedArea = this.getItem('selectedArea');
    console.log("Localstorage selectedArea from generic service:", selectedArea);
    
    if (selectedArea) {
      if (typeof selectedArea === 'object' && 'name' in selectedArea && 'id' in selectedArea) {
        return await this.checkAreaExists(selectedArea.id);
      } else {
        console.error("Data format is incorrect:", selectedArea);
        this.removeItem('selectedArea');
        return false;
      }
    } else {
      console.warn("No data found for 'selectedArea'");
      return false;
    }
  }
  
  async checkAreaExists(areaID: number): Promise<boolean> {
    try {
      const result: Area | boolean | undefined = await this.service.exists('Area', areaID).toPromise();
      if (result === null || result === undefined || typeof result === 'boolean') {
        console.log("Handling the 404 scenario.");
        this.removeItem('selectedArea');
        this.router.navigate(['/areapick']);
        return false;
      } else {
        const formattedResult = {
          id: result.areaID,
          name: result.areaName
        };
        console.log("formattedResult:", formattedResult);
        this.setItem('selectedArea', formattedResult);
        return true;
      }
    } catch (error) {
      this.removeItem('selectedArea');
      this.router.navigate(['/areapick']);
      console.error("An error occurred:", error);
      return false;
    }
  }
  
  

  handleStorageChange(event: StorageEvent): void {
    console.log("LocalStorage change detected:", event)
    if (event.key === 'selectedArea') {
      console.log('LocalStorage change detected:', event);
    }
  }

  setItem(key: string, value: any): void {
    localStorage.setItem(key, JSON.stringify(value));
  }

  getItem(key: string): any {
    const item = localStorage.getItem(key);
    return item ? JSON.parse(item) : null;
  }

  removeItem(key: string): void {
    localStorage.removeItem(key);
  }

  clear(): void {
    localStorage.clear();
  }

}

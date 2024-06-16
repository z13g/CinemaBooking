import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BookShow } from '../models/BookShow/bookshow';

@Injectable({
  providedIn: 'root'
})
export class BookShowService {
  private bookShowSource = new BehaviorSubject<BookShow | null>(this.getBookShowFromSession());
  currentBookShow = this.bookShowSource.asObservable();

  constructor() {}

  updateBookShow(bookShow: BookShow) {
    this.bookShowSource.next(bookShow);
    this.saveBookShowToSession(bookShow);
  }

  private saveBookShowToSession(bookShow: BookShow) {
    sessionStorage.setItem('currentBookShow', JSON.stringify(bookShow));
  }

  private getBookShowFromSession(): BookShow | null {
    const bookShowJson = sessionStorage.getItem('currentBookShow');
    return bookShowJson ? JSON.parse(bookShowJson) : null;
  }

  clearBookShow() {
    this.bookShowSource.next(null);
    sessionStorage.removeItem('currentBookShow');
  }
}

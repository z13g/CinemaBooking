import { TestBed } from '@angular/core/testing';

import { BookshowService } from './bookshow.service';

describe('BookshowService', () => {
  let service: BookshowService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookshowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMovieComponent } from './adminmovie.component';

describe('AdminMovieComponent', () => {
  let component: AdminMovieComponent;
  let fixture: ComponentFixture<AdminMovieComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminMovieComponent]
    });
    fixture = TestBed.createComponent(AdminMovieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

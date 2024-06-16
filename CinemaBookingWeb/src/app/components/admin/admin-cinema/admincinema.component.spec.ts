import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCinemaComponent } from './admincinema.component';

describe('AdminCinemaComponent', () => {
  let component: AdminCinemaComponent;
  let fixture: ComponentFixture<AdminCinemaComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminCinemaComponent]
    });
    fixture = TestBed.createComponent(AdminCinemaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdmincinemaHallComponent } from './admincinemahall.component';

describe('AdmincinemaHallComponent', () => {
  let component: AdmincinemaHallComponent;
  let fixture: ComponentFixture<AdmincinemaHallComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdmincinemaHallComponent]
    });
    fixture = TestBed.createComponent(AdmincinemaHallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

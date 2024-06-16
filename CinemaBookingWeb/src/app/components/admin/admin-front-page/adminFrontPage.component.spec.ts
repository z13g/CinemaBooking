import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminFrontPageComponent } from './adminFrontPage.component';

describe('AdminFrontPageComponent', () => {
  let component: AdminFrontPageComponent;
  let fixture: ComponentFixture<AdminFrontPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminFrontPageComponent]
    });
    fixture = TestBed.createComponent(AdminFrontPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

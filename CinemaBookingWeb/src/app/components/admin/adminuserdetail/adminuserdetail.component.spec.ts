import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminuserdetailComponent } from './adminuserdetail.component';

describe('AdminuserdetailComponent', () => {
  let component: AdminuserdetailComponent;
  let fixture: ComponentFixture<AdminuserdetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminuserdetailComponent]
    });
    fixture = TestBed.createComponent(AdminuserdetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

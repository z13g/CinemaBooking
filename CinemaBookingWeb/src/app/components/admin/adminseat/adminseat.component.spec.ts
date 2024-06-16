import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminseatComponent } from './adminseat.component';

describe('AdminseatComponent', () => {
  let component: AdminseatComponent;
  let fixture: ComponentFixture<AdminseatComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminseatComponent]
    });
    fixture = TestBed.createComponent(AdminseatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

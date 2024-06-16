import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminshowComponent } from './adminshow.component';

describe('AdminshowComponent', () => {
  let component: AdminshowComponent;
  let fixture: ComponentFixture<AdminshowComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminshowComponent]
    });
    fixture = TestBed.createComponent(AdminshowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

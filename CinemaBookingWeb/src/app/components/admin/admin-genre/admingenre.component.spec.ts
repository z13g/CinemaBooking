import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdmingenreComponent } from './admingenre.component';

describe('AdmingenreComponent', () => {
  let component: AdmingenreComponent;
  let fixture: ComponentFixture<AdmingenreComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdmingenreComponent]
    });
    fixture = TestBed.createComponent(AdmingenreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

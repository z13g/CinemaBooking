import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminregionComponent } from './adminregion.component';

describe('AdminregionComponent', () => {
  let component: AdminregionComponent;
  let fixture: ComponentFixture<AdminregionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminregionComponent]
    });
    fixture = TestBed.createComponent(AdminregionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

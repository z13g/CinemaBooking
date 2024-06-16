import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AreapickComponent } from './areapick.component';

describe('AreapickComponent', () => {
  let component: AreapickComponent;
  let fixture: ComponentFixture<AreapickComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AreapickComponent]
    });
    fixture = TestBed.createComponent(AreapickComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

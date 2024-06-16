import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostumerComponent } from './costumer.component';

describe('CostumerComponent', () => {
  let component: CostumerComponent;
  let fixture: ComponentFixture<CostumerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CostumerComponent]
    });
    fixture = TestBed.createComponent(CostumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

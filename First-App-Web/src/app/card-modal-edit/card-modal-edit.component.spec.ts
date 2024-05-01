import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardModalEditComponent } from './card-modal-edit.component';

describe('CardModalEditComponent', () => {
  let component: CardModalEditComponent;
  let fixture: ComponentFixture<CardModalEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardModalEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CardModalEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardModalCreateComponent } from './card-modal-create.component';

describe('CardModalCreateComponent', () => {
  let component: CardModalCreateComponent;
  let fixture: ComponentFixture<CardModalCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardModalCreateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CardModalCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

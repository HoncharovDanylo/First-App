import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryGeneralComponent } from './history-general.component';

describe('HistoryGeneralComponent', () => {
  let component: HistoryGeneralComponent;
  let fixture: ComponentFixture<HistoryGeneralComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HistoryGeneralComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HistoryGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

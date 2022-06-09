import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientStatisticsComponent } from './client-statistics.component';

describe('ClientStatisticsComponent', () => {
  let component: ClientStatisticsComponent;
  let fixture: ComponentFixture<ClientStatisticsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientStatisticsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartDocsComponent } from './chart-docs.component';

describe('ChartDocsComponent', () => {
  let component: ChartDocsComponent;
  let fixture: ComponentFixture<ChartDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

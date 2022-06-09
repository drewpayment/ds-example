import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChipsDocsComponent } from './chips-docs.component';

describe('ChipsDocsComponent', () => {
  let component: ChipsDocsComponent;
  let fixture: ComponentFixture<ChipsDocsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChipsDocsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChipsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

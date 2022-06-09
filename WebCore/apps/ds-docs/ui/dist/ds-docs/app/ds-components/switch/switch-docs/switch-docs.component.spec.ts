import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwitchDocsComponent } from './switch-docs.component';

describe('SwitchDocsComponent', () => {
  let component: SwitchDocsComponent;
  let fixture: ComponentFixture<SwitchDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwitchDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwitchDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlControlComponent } from './client-gl-control.component';

describe('ClientGlControlComponent', () => {
  let component: ClientGlControlComponent;
  let fixture: ComponentFixture<ClientGlControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

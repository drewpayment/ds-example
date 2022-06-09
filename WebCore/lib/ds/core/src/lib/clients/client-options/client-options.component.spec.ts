import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientOptionsComponent } from './client-options.component';

describe('ClientOptionsComponent', () => {
  let component: ClientOptionsComponent;
  let fixture: ComponentFixture<ClientOptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientOptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientOptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

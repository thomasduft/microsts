import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ScopeListComponent } from './scope-list.component';

describe('ScopeListComponent', () => {
  let component: ScopeListComponent;
  let fixture: ComponentFixture<ScopeListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ScopeListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScopeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

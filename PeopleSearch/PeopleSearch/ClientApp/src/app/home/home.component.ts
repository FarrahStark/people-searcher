import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl, ValidatorFn, AbstractControl } from '@angular/forms';
import { debounce, map, tap, switchMap, publish, refCount } from 'rxjs/operators';
import { timer, Subscription } from 'rxjs';
import { PersonRepositoryService } from '../services/person-repository.service';
import { Person, PeopleSearchResponse } from '../models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit, OnDestroy {
  displayedColumns = ['first', 'middle', 'last', 'age'];
  searchBox: FormControl = new FormControl();
  isSearching = false;
  selectedPerson?: Person;
  searchResults: Person[] = [];

  private getSearchResultsSubscription?: Subscription;

  constructor(private personRepository: PersonRepositoryService) {
  }

  ngOnInit(): void {
    this.searchBox = new FormControl('', [this.validateSearch()]);
    const getSearchResultsPromo = this.searchBox.valueChanges
      .pipe(
        debounce(() => timer(400)),
        tap(() => this.isSearching = true),
        tap(() => this.selectedPerson = undefined),
        switchMap(() => {
          const searchText = this.searchBox.value;
          return this.personRepository.runSearch(searchText, 100);
        }),
        tap(() => this.isSearching = false),
        publish(),
        refCount()
      );

      this. getSearchResultsSubscription = getSearchResultsPromo.subscribe(
      (searchResults: PeopleSearchResponse) => {
        this.searchResults = searchResults.MatchingPeople;
      });
  }

  ngOnDestroy(): void {
    if (this.getSearchResultsSubscription !== undefined) {
      try {
        this.getSearchResultsSubscription.unsubscribe();
      } finally {
        this.getSearchResultsSubscription = undefined;
      }
    }
  }

  validateSearch(): ValidatorFn {
    return (internalControl: AbstractControl): { [key: string]: any } | null => {
      const isValid = true;
      return isValid ? null : { 'InvalidControl': true };
    };
  }
}

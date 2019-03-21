import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { FormControl, ValidatorFn, AbstractControl, Validators } from '@angular/forms';
import { debounce, map, tap, switchMap, publish, refCount, skipWhile, withLatestFrom, startWith } from 'rxjs/operators';
import { timer, Subscription, combineLatest } from 'rxjs';
import { PersonRepositoryService } from '../services/person-repository.service';
import { Person, PeopleSearchResponse } from '../models';
import { skipIf } from '../operators';
import { DrawerService } from '../services/drawer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  displayedColumns = ['first', 'middle', 'last', 'age'];
  searchBox: FormControl = new FormControl();
  delayInput: FormControl = new FormControl();
  delayMin = 0;
  delayMax = 10000;
  isSearching = false;
  selectedPerson?: Person;
  searchResults: Person[] = [];
  searched = false;
  private screenWidth: number = 0;
  private screenHeight: number = 0;
  get isMobile(): boolean {
    return this.screenWidth <= 768;
  }

  get drawerMode(): string {
    return this.isMobile ? 'side' : 'side';
  }

  get showDetails(): boolean {
    return this.selectedPerson !== undefined;
  }
  get noResults() {
    return this.searchResults.length < 1 && this.searched && !this.searchBox;
  }

  private getSearchResultsSubscription?: Subscription;

  constructor(
    private personRepository: PersonRepositoryService,
    public drawerService: DrawerService) {
  }

  ngOnInit(): void {
    const startingDelay = 100;
    this.searchBox = new FormControl('', [this.validateSearch()]);
    this.delayInput = new FormControl(startingDelay, [
      Validators.min(this.delayMin), Validators.max(this.delayMax)
    ]);
    const delayObservable = this.delayInput.valueChanges.pipe(
      startWith(startingDelay)
    );
    const getSearchResultsPromo = combineLatest(this.searchBox.valueChanges, delayObservable)
      .pipe(
        debounce(() => timer(400)),
        skipIf(([searchText, delayMilliseconds]) => {
          return !this.isValidSearch(searchText);
        }),
        tap(() => {
          this.isSearching = true;
          this.selectedPerson = undefined;
          this.searchResults = [];
          this.searched = true;
        }),
        switchMap(([searchText, delayMilliseconds]) => {
          console.log(`Searching with '${searchText}'`);
          return this.personRepository.runSearch(searchText, delayMilliseconds);
        }),
        tap(() => this.isSearching = false),
        publish(),
        refCount()
      );

      this. getSearchResultsSubscription = getSearchResultsPromo.subscribe(
      (searchResults: PeopleSearchResponse) => {
        this.searchResults = searchResults.matchingPeople;
      },
      (error: any) => {
        this.isSearching = false;
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

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
      this.screenWidth = window.innerWidth;
      this.screenHeight = window.innerHeight;
  }

  personSelected(person: Person) {
    this.deselectPerson();
    setTimeout(() => {
      this.selectedPerson = person;
      this.drawerService.isOpen = true;
    });
  }

  deselectPerson() {
    this.selectedPerson = undefined;
    this.drawerService.isOpen = false;
  }

  isValidSearchText(text: string): boolean {
    const validSearchRegex = /^\s*(?:[a-zA-Z]+\s+){1,2}[a-zA-Z]+\s*$/g;
    return validSearchRegex.test(text);
  }

  isValidSearch(text: string): boolean {
    const delay = this.delayInput.value;
    return this.isValidSearchText(text) &&
      delay >= this.delayMin &&
      delay <= this.delayMax;
  }

  validateSearch(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const invalid = { 'InvalidControl': true };
      if (!control || !control.value) {
        return invalid;
      }

      const blankStringRegex = /^\s*$/;

      const searchText = control.value;
      const isValid = this.isValidSearchText(searchText) ||
        blankStringRegex.test(searchText);
      return isValid ? null : invalid;
    };
  }
}

export interface Note {
    id: number;
    name: string;
    description: string;
  }
  
  export interface State {
    notes: Note[];
  }
  
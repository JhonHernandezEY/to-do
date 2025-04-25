import { createSlice } from '@reduxjs/toolkit';

const todoSlice = createSlice({
  name: 'todos',
  initialState: [],
  reducers: {
    addTodo: (state, action) => {
      state.push({ text: action.payload, completed: false });
    },
    toggleTodo: (state, action) => {
      const todo = state[action.payload];
      todo.completed = !todo.completed;
    },
    deleteTodo: (state, action) => {
      return state.filter((_, index) => index !== action.payload);
    },
  },
});

export const { addTodo, toggleTodo, deleteTodo } = todoSlice.actions;
export default todoSlice.reducer;

// features/todoSlice.js
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';

const API_URL = 'https://jsonplaceholder.typicode.com/todos';

// Sample deadlines for demonstration purposes
const sampleDeadlines = [
    '2023-10-01T12:00', // Example deadline for the first todo
    '2023-10-02T15:30', // Example deadline for the second todo
    '2023-10-03T09:00', // Example deadline for the third todo
    '2023-10-04T18:00', // Example deadline for the fourth todo
];

const initialState = [];

// Async thunk to fetch todos from JSONPlaceholder and add deadlines
export const fetchTodos = createAsyncThunk('todos/fetchTodos', async () => {
    const response = await axios.get(API_URL);
    // Map over the fetched data to add a deadline
    return response.data.slice(0, 10).map((todo, index) => ({
        ...todo,
        deadline: sampleDeadlines[index % sampleDeadlines.length], // Assign a deadline from the sample array
    }));
});

export const addTodoAsync = createAsyncThunk('todos/addTodo', async ({ todoText, deadline }) => {
    const response = await axios.post(API_URL, {
        title: todoText,
        completed: false,
        deadline: deadline, 
    });
    return response.data; 
});

export const deleteTodoAsync = createAsyncThunk('todos/deleteTodo', async (id) => {
    await axios.delete(`${API_URL}/${id}`);
    return id;
});

const todoSlice = createSlice({
    name: 'todos',
    initialState, // Use the updated initial state
    reducers: {
        toggleTodo: (state, action) => {
            const todo = state.find((todo) => todo.id === action.payload);
            if (todo) {
                todo.completed = !todo.completed;
            }
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchTodos.fulfilled, (state, action) => {
                return action.payload; // Set the state with modified todos
            })
            .addCase(addTodoAsync.fulfilled, (state, action) => {
                state.push(action.payload);
            })
            .addCase(deleteTodoAsync.fulfilled, (state, action) => {
                return state.filter((todo) => todo.id !== action.payload);
            });
    },
});

export const { toggleTodo } = todoSlice.actions;
export default todoSlice.reducer;

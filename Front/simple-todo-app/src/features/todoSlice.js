import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';

const API_URL = 'https://jsonplaceholder.typicode.com/todos'; // New: Define the API URL

export const fetchTodos = createAsyncThunk('todos/fetchTodos', async () => {
    const response = await axios.get(API_URL);
    return response.data.slice(0, 10); // Limit to 10 items for simplicity
});

export const addTodoAsync = createAsyncThunk('todos/addTodo', async (todoText) => {
    const response = await axios.post(API_URL, {
        title: todoText,
        completed: false,
    });
    return response.data; 
});

export const deleteTodoAsync = createAsyncThunk('todos/deleteTodo', async (id) => {
    await axios.delete(`${API_URL}/${id}`);
    return id; 
});

const todoSlice = createSlice({
    name: 'todos',
    initialState: [],
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
                return action.payload; 
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

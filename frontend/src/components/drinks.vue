<template>
  <div class="container">
    <h1 class="title">Máquina Expendedora</h1>

    <div class="drink-card" v-for="drink in drinks" :key="drink.name">
      <div class="drink-info">
        <h2>{{ drink.name }}</h2>
        <p>₡{{ drink.price }}</p>
        <p>Disponibles: {{ drink.quantity }}</p>
      </div>
      <div class="actions">
        <div class="counter">
          <button @click="decrease(drink)" :disabled="quantities[drink.name] <= 0">−</button>
          <span class="count">{{ quantities[drink.name] }}</span>
          <button @click="increase(drink)" :disabled="quantities[drink.name] >= drink.quantity">+</button>
        </div>
      </div>
    </div>

    <!-- Carrito -->
    <div class="cart">
      <h2>🛒 Carrito</h2>
      <div v-if="cartItems.length === 0">
        <p>Selecciona bebidas para ver el resumen aquí.</p>
      </div>
      <ul v-else>
        <li v-for="item in cartItems" :key="item.name">
          {{ item.name }} x {{ item.quantity }} = ₡{{ item.quantity * item.price }}
        </li>
      </ul>
      <div v-if="cartItems.length > 0" class="total">
        Total: ₡{{ total }}
      </div>
      <button v-if="cartItems.length > 0" @click="payCart" class="pay-button">
        Pagar todo
      </button>
    </div>
  </div>
</template>


<script setup>
import '../assets/drinks.css'
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'

const drinks = ref([])
const quantities = ref({})
const cart = ref(Object.create(null))

const fetchDrinks = async () => {
  const res = await axios.get('http://localhost:5110/api/drinks')
  drinks.value = res.data
  res.data.forEach(drink => {
    quantities.value[drink.name] = 0
  })
}

const updateCart = (drink) => {
  cart.value[drink.name] = {
    name: drink.name,
    quantity: quantities.value[drink.name],
    price: drink.price
  }
}

const increase = (drink) => {
  const current = quantities.value[drink.name] || 0
  if (current < drink.quantity) {
    quantities.value[drink.name] = current + 1
    updateCart(drink)
  }
}

const decrease = (drink) => {
  const current = quantities.value[drink.name] || 0
  if (current > 0) {
    quantities.value[drink.name] = current - 1
    updateCart(drink)
  }
}


const cartItems = computed(() =>
  Object.values(cart.value).filter(item => item.quantity > 0)
)

const total = computed(() =>
  cartItems.value.reduce((sum, item) => sum + item.quantity * item.price, 0)
)

const payCart = async () => {
  try {
    for (const item of cartItems.value) {
      await axios.post('http://localhost:5110/api/drinks/purchase', {
        name: item.name,
        quantity: item.quantity
      })
    }

    alert('¡Compra completada con éxito!')
    await fetchDrinks()
    Object.keys(cart.value).forEach(name => {
      cart.value[name].quantity = 0
    })
  } catch (err) {
    alert('Error al procesar el pago: ' + (err.response?.data || err.message))
  }
}

onMounted(fetchDrinks)
</script>

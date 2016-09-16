# RC4Ever
RC4 stream cipher variants for hashing, CBC, visualizing how it works, or just general encryption.

Includes 2 variants: 
1) A simple table with a method to visualize the permutation state of the table and the avalanche effect as a bitmap
2) A more serious attempt at a secure encryption scheme. **NOTE: PROBABLY NOT ACTUALLY SECURE, SO DO NOT TRUST IT!**

The class called SimpleTable is exactly that; The simplest R4C implementation possible. It is notable in the fact that it does not use key scheduling at all, and its starting state is that of the Identity Permutation. The identity permutation is where the value at index zero equals zero, the value at index one is one, and so on. An easy way to remember what the Identity Permutation is, just recall the notion of a Multiplicative Identity (which is 1), where by multiplying a number N by the Multiplicative Identity gives you back the value of N. Similarly, the Identity Permutation of N just gives you N. That is, there is no permutation being applied to the array at all.

This is done to see the perfectly ordered state, and how each round effects that state. In this way, we can visually check for the avalanche effect. In order to visualize the table, i just assign each value 0 to 255 a different shade of grey (I also have a rainbow-colored option that might be easier to tell apart similar values). At each step I create a Bitmap by looping through the table. Below, you can the first 100 steps of this cipher being applied to the table:
<img border="0" height="200" src="https://github.com/AdamRakaska/RC4Ever/blob/master/rc4-first-200-steps_fast.gif" width="200" />

Notice how it takes a while to get going and the first several values don't move much at all. After 256 steps, or one round, the cursor arrives back at index zero. Because the location of the first several values have not moved much or at all, we can clearly see that a mere 256 steps is insufficient at permuting the state enough to avoid leaking the first part of your key. Therefore it it is vital to permute the table for several rounds (256 steps per round) before you start using the stream.

This is the base project from which other projects currently in progress are cloned from.

![alt text](https://github.com/AdamRakaska/RC4Ever/blob/master/RC4Ever2.JPG "RC4Ever Screenshot")
